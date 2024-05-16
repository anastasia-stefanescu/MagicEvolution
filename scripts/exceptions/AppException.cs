using Godot;
using System;

public class AppException : Exception {
    public AppException() : base() {}
    public AppException(String message) : base(message) {}
    public AppException(String message, Exception inner) : base(message, inner) {} 
}