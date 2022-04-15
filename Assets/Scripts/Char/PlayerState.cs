using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerState {
    public interface IPlayerState
    {
        void onStateEnter();
        void onStateUpdate();
        void onStateExit();
    }

    public class IdleState : IPlayerState {
        public void onStateEnter(){

        }
        public void onStateUpdate(){

        }
        public void onStateExit(){

        }
    }

    public class CombatState : IPlayerState {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class PeaceState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class FallState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class InteractionState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class WalkState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class CrawlState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class ReadyToJumpState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class JumpState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class ReadyToRideState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class RideState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class UltAttackState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }

    public class AttackState : IPlayerState
    {
        public void onStateEnter()
        {

        }
        public void onStateUpdate()
        {

        }
        public void onStateExit()
        {

        }
    }
}