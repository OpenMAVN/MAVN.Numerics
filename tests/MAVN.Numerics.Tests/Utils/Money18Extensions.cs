namespace MAVN.Numerics.Tests.Utils
{
    public static class Money18Extensions
    {
        public static Money18Assertions Should(
            this Money18 instance)
        {
            return new Money18Assertions(instance); 
        } 
    }
}