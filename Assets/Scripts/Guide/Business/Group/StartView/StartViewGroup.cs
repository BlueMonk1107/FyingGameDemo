using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartViewGroup : GuideBehaviourGroupBase {
    public StartViewGroup(int parentId) : base(parentId)
    {
    }

    protected override bool IsTrigger {
        get { return true; }
    }
    protected override int GroupId { get { return 3; } }
    protected override Queue<IGuideBehaviour> GetGuideItems()
    {
        Queue<IGuideBehaviour> behaviours = new Queue<IGuideBehaviour>();
        behaviours.Enqueue(new WelcomeGuideBehaviour());
        behaviours.Enqueue(new StartGameBehaviour());
        return behaviours;
    }
}

