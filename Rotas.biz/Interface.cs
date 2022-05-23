using Rotas.data;


namespace Rotas.biz
{
    public static class Interface
    {
        public static IRota CreateRota() { return ((IRota)new negocio.Rota()); }
    }
}
