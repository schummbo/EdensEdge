using EdensEdge.Exceptions;
using Godot;

namespace EdensEdge.Extensions
{
    public static class NodeExtensions
    {
        public static void CheckRequiredDependency(this Node node, Node requiredNode, string containerName)
        {
            if (requiredNode == null)
            {
                throw new DependencyMissingException(containerName, requiredNode.Name);
            }
        }
    }
}
