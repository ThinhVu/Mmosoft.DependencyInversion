namespace Mmosoft.DependencyInversion
{
    public enum RegisterOption
    {
        Cache, // create only one instance, cached it for later resolve
        NoCache // create instance every time
    }
}
