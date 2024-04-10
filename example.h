#ifndef TEST
#define TEST

#include <godot_cpp/classes/node2d.hpp>

using namespace godot;

class Example : public Node2D {
    GDCLASS(Example, Node2D)

protected:
    static void _bind_methods();

public:
    void exampleBoundMethod();
};

#endif