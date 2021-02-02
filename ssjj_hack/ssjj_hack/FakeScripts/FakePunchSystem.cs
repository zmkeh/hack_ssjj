using Assets.Sources.Framework;
using Entitas;

namespace ssjj_hack
{

    public class FakePunchSmoothSystem : IAfterPredicationSystem
    {
        public FakePunchSmoothSystem(Contexts contexts)
        {
            this._players = contexts.player.GetGroup(PlayerMatcher.AllOf(new IMatcher<PlayerEntity>[]
            {
                PlayerMatcher.CameraOwner
            }));
            this._timeContext = contexts.time;
        }

        public void OnAfterPredication()
        {
            foreach (PlayerEntity playerEntity in this._players)
            {
                if (playerEntity.isPrediction)
                {
                    this.DoSmooth(playerEntity);
                }
            }
        }

        private void DoSmooth(PlayerEntity entity)
        {
            entity.punchSmooth.TempPunchYaw = entity.punchOrientation.PunchYaw * 2;
            entity.punchSmooth.TempPunchPitch = entity.punchOrientation.PunchPitch * 2;
        }

        private readonly IGroup<PlayerEntity> _players;

        private readonly TimeContext _timeContext;
    }
}
