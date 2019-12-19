﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ViewBase,IReceiver {
    
    private ItemCDEffect _cdEffect;
    private int _count;
    private ItemEffect _itemEffect;
    
    protected override void InitChild()
    {
        UpdateCount();
        var effect = LoadMgr.Single.LoadPrefabAndInstantiate(Paths.PREFAB_ITEM_EFFECT_VIEW,transform);
        _itemEffect = effect.AddComponent<ItemEffect>();
        _cdEffect = Util.Get("Mask").Add<ItemCDEffect>();
        _cdEffect.SetShow();
    }
    
    public override void Show()
    {
        base.Show();
        MessageMgr.Single.AddListener(MsgEvent.EVENT_BOMB,this);
        MessageMgr.Single.AddListener(MsgEvent.EVENT_USE_BOMB,this);
        MessageMgr.Single.AddListener(MsgEvent.EVENT_CHANGE_HAND,this);
        _count = GameModel.Single.BombCount;
        ReceiveMessage();
    }

    public override void Hide()
    {
        base.Hide();
        MessageMgr.Single.RemoveListener(MsgEvent.EVENT_BOMB,this);
        MessageMgr.Single.RemoveListener(MsgEvent.EVENT_USE_BOMB,this);
        MessageMgr.Single.RemoveListener(MsgEvent.EVENT_CHANGE_HAND,this);
    }
    
    private void UpdateCount()
    {
        Util.Get("Num").SetText(GameModel.Single.BombCount);
    }

    public void ReceiveMessage(params object[] args)
    {
        UpdateCount();
        UpdateState();
        UpdateHandState();
    }
    
    private void UpdateState()
    {
        if (GameModel.Single.BombCount == 0)
        {
            _cdEffect.SetMask();
            _itemEffect.SetActive(false);
        }
        else
        {
            if (_count > GameModel.Single.BombCount)
            {
                _cdEffect.StartCD(()=>_itemEffect.SetActive(true));
                _count = GameModel.Single.BombCount;
                _itemEffect.SetActive(false);
            }
        }
    }
    
    private void UpdateHandState()
    {
        GameUtil.ChangeHandPos(transform.Rect());
    }
}
