namespace Game.Scripts.Base.Services.ABTest
{
    public interface IABTestService
    {
        void Init();
        bool CheckTestGroup(string testName, string groupName);
    }
}