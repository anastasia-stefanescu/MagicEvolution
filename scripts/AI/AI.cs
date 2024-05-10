using Godot;
using System;
using System.Threading;

public abstract partial class AI : ManaConsumer {
    protected Vision visionNode=null;

    public abstract AI_Output run(AI_Input input);

    public VisionData getVisionData() {
        if(visionNode == null)
            throw new AppException("Error in AI: visionNode is null. Make sure the vision node was constructed before call (preferably in child class' constructor and/or in its _enter_tree function).");
        return visionNode.getVisionData();
    }
}
