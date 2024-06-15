using System.Net;

namespace ApiPeliculas.Modelos
{
    public class RespiuestaAPI
    {
        public RespiuestaAPI()
        {
            ErrorMesasages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucess {  get; set; }
        public List<string> ErrorMesasages { get; set; }
        public object Result { get; set; }
    }
}
