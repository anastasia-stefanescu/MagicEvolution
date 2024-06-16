using Godot;
using System;

public partial class Vision : Godot.Node2D {
	private VisionGenome genome;
	private double fov; // in degrees
	private uint rayCount;
	private double range;

	private RayCast2D[] rayNodes;

	// completam cu ce se vede din RayNodes un VisionData cu: distanta si unghiul la care vede ceva, si daca ce vede e Wizbit sau Mana
	public VisionData getVisionData() {
		if(rayCount==0)
			return new VisionData(0);
		
		// init data
		VisionData data = new VisionData(genome.calcRayCount());
		double baseAngle = 0; // the angle in degrees between two (spacially) neighbouring rays
		if(rayCount>1)
			baseAngle=fov/(rayCount-1)*2;
		
		for(uint i=0; i<data.rayCount; i++) {
			
			// set distance field
			if(rayNodes[i].IsColliding()) {
				data.raysData[i].distance = rayNodes[i].GlobalPosition.DistanceTo(rayNodes[i].GetCollisionPoint()) / range;
			}
			else
				data.raysData[i].distance=1;
			
			// set angle field
			if(i==0)
				data.raysData[i].angle=0;
			else {
				data.raysData[i].angle=baseAngle*((i+1)/2);
				if(i%2==1)
					data.raysData[i].angle*=-1;
			}
			
			// set identification fields
			if(rayNodes[i].IsColliding()) {
				if( rayNodes[i].GetCollider() as Wizbit != null ) { // is wizbit check
					data.raysData[i].isWizbit=1;
					data.raysData[i].isMana=0;
				}
				else if( rayNodes[i].GetCollider() as Mana != null) { // is mana check
					data.raysData[i].isWizbit=0;
					data.raysData[i].isMana=1;
				}
			}
			else {
				data.raysData[i].isWizbit=0;
				data.raysData[i].isMana=0;
			}
		}

		return data;
	}

	// this is only public because AI implementations need to see it
	public void updateGenome(VisionGenome newGenome) {
		genome=(VisionGenome)newGenome.clone();
		generate();
	}

	private void generate() {
		fov=genome.getFOV()*180;
		rayCount=genome.calcRayCount();
		range=genome.getRange();
		rayNodes=null;
		
		// Instantiate raycast nodes
		initRaycastNodes();
	}

	public Vision(VisionGenome genome) {
		this.genome=genome;
		generate();
	}
	
	public uint getRayCount(){
		return rayCount;
	}

	public RayCast2D[] getRays(){
		return rayNodes;
	}

	//initializam RayCast2D-urile unghiul de la care incepe range-ul de viziune, distanta pe care pot vedea, si sa poata vedea si Mana
	private void initRaycastNodes() {
		if(rayCount==0||rayNodes!=null)
			return;
		rayNodes=new RayCast2D[rayCount];
		rayNodes[0]=new RayCast2D();
		rayNodes[0].TargetPosition=new Vector2(0, (float)-range); 
		rayNodes[0].CollideWithAreas=true; // for mana detection
		AddChild(rayNodes[0]);
		float angle=0;
		for(uint i=1;i<rayCount;i+=2) {
			angle+=(float)fov/((float)(rayCount-1)/2);
			
			rayNodes[i]=new RayCast2D();
			rayNodes[i].TargetPosition=new Vector2(-Mathf.Cos(Mathf.DegToRad(90-angle)), -Mathf.Sin(Mathf.DegToRad(90-angle))) * (float)range;
			rayNodes[i].CollideWithAreas=true; // for mana detection
			AddChild(rayNodes[i]);
			
			rayNodes[i+1]=new RayCast2D();
			rayNodes[i+1].TargetPosition=new Vector2(Mathf.Cos(Mathf.DegToRad(90-angle)), -Mathf.Sin(Mathf.DegToRad(90-angle))) * (float)range;
			rayNodes[i+1].CollideWithAreas=true; // for mana detection
			AddChild(rayNodes[i+1]);
		}
	}
}
