namespace IdempotentApi.Domain
{
    public class IdempotencyResponse
    {
        public Object? Result { get; set; }

        public int IdemPotencyStatusCode { get; set; }
    }
}
