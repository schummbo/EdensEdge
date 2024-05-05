using System;

namespace EdensEdge.Exceptions;

public class DependencyMissingException : Exception
{
    public DependencyMissingException(string nodeName, string dependencyName) 
        : base($"Missing dependency. Please set it on editor. Node: {nodeName}, Depedency: {dependencyName}")
    {
    }
}
