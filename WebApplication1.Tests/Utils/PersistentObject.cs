namespace TestTaskApp.Domain.Domain.Utils
{
    public abstract class PersistentObject<T>
    {
        public virtual T Id { get; set; }

        public virtual bool IsPersistent
        {
            get { return (!Id.Equals(default(T))); }
        }

        public override bool Equals(object obj)
        {
            if (IsPersistent)
            {
                if (obj == null)
                {
                    return false;
                }

                if (GetType() != obj.GetType())
                {
                    return false;
                }

                var persistentObject = obj as PersistentObject<T>;
                return (persistentObject != null) && (this.Id.Equals(persistentObject.Id));
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return IsPersistent ? Id.GetHashCode() : base.GetHashCode();
        }
    }
}