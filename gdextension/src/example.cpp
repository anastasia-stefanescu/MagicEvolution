#include "example.h"
#include <godot_cpp/variant/utility_functions.hpp>

void Example::_bind_methods() {
    ClassDB::bind_method(D_METHOD("exampleBoundMEthod"), &Example::exampleBoundMethod);
}

void Example::exampleBoundMethod() {
    UtilityFunctions::print("Hello world from Example class!\n");
}