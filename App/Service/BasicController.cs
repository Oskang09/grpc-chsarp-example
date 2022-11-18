using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace amantiq.service
{
    public class BasicController : Controller
    {

        [Service("migration")]
        public void migration(Context request)
        {
            using (var db = GetDatabase)
            {
                db.Database.Migrate();
            }
        }

        [Service("health")]
        public object health(Context request)
        {
            return true;
        }
    }

}