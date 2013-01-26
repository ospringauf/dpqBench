namespace Paguru.DpBench.Renderer
{
    using Paguru.DpBench.Model;

    public interface IRenderer
    {
        object Render(GroupFilter gl);
    }
}