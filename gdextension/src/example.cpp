#include "example.h"
#include <godot_cpp/variant/utility_functions.hpp>

void Example::_bind_methods() {
    ClassDB::bind_method(D_METHOD("exampleBoundMethod"), &Example::exampleBoundMethod);
    ClassDB::bind_method(D_METHOD("secondBoundMethod"), &Example::secondBoundMethod);
}

void Example::exampleBoundMethod() {
    exampleChild=new Example();
    add_child(exampleChild);
    exampleChild->secondBoundMethod();
    UtilityFunctions::print("Hello world from test Example class!\n");
}

void Example::secondBoundMethod() {
    UtilityFunctions::print("Hello world from secondExampleMethod!\n");
}
