#include "pch.h"
#include "GameScene.h"
#include "Agent.h"

void GameScene::Init()
{
	for (int i = 0; i < 100; i++)
	{
		Object* testObj = new Agent;
		AddObject(testObj, LAYER::ENEMY);
	}
}
