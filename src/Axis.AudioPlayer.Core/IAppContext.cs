using System.Threading.Tasks;

namespace Axis.AudioPlayer
{
    public interface IAppContext
    {
        Task LoadContext();
        Task SaveContext();
    }
}