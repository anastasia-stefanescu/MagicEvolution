using Godot;
using System;

public static class SimulationParameters {

    public static class AIParameters {

        public static class MutationChances {

        }

        public static MC_WeightFunctionEnum mc_weightFunction = MC_WeightFunctionEnum.Quadratic;

    }


    public static void resetToDefault() {
        GD.Print("Warning! SimulationParameters::resetToDefault() currently does nothing!");
    }
}