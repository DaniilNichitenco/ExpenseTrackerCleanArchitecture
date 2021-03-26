using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Domain.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(Type entityType, int id) : base($"Entity of type { entityType.Name } with id: { id } not found")
        {
        }
        public EntityNotFoundException(Type entityType, int parentId, int childId)
            : base($"Entity of type { entityType.Name } with parent ID: { parentId } and child ID: { childId } not found")
        {
        }
        public EntityNotFoundException(Type entityType) : base($"Entity of type { entityType.Name } not found")
        {
        }
        public static EntityNotFoundException OfType<T>(int withId)
        {
            return new EntityNotFoundException(typeof(T), withId);
        }
        public static EntityNotFoundException OfType<T>(int parentID, int childID)
        {
            return new EntityNotFoundException(typeof(T), parentID, childID);
        }
        public static EntityNotFoundException OfType<T>()
        {
            return new EntityNotFoundException(typeof(T));
        }
    }
}
