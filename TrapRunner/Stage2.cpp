#include "pch.h"
#include "Stage2.h"
#include "CollisionManager.h"
#include "Agent.h"
#include "ResourceManager.h"
#include "RollingSkillUI.h"
#include "UIManager.h"

#include "TileObject.h"
#include "FallTileObject.h"
#include "TrapTileObject.h"
#include "FollowTrap.h"
#include "StarLazer.h"
#include "MagicTower.h"
#include "ConditionTile.h"
#include "Button.h"
#include "Direction.h"
#include "EndingManager.h"
#include "Gold.h"

#define offset 128;

#define left(v)		v = {v.x - 256, v.y}
#define right(v)	v = {v.x + 256, v.y}
#define up(v)		v = {v.x, v.y - 256}
#define down(v)		v = {v.x, v.y + 256}

#define leftm(v, m)			v = {v.x - 256 * m, v.y}
#define rightm(v, m)		v = {v.x + 256 * m, v.y}
#define upm(v, m)			v = {v.x, v.y - 256 * m}
#define downm(v, m)			v = {v.x, v.y + 256 * m}

#define move(v, xm, ym)		v = {v.x + 256 * xm, v.y + 256 * ym} 
#define moveSquareRight(v)	v = {v.x + 512, v.y + 256}
#define moveRight(v)		v = {v.x + 256, v.y + 256}

#define CreateSquare(v, t1,t2,t3,t4)\
Stage2::CreateObject(v, t1);		\
v = right(v);						\
Stage2::CreateObject(v, t2);		\
v = up(v);							\
Stage2::CreateObject(v, t3);		\
v = left(v);						\
Stage2::CreateObject(v, t4)			\

#define CreateUpDown(v, t1, t2)	\
Stage2::CreateObject(v, t1);	\
v = up(v);						\
Stage2::CreateObject(v, t2)		\

void Stage2::CreateArrow(Vec2 v, wstring d)
{
	Direction* dir = new Direction(d);
	dir->GetTransform()->SetPosition(v);
	AddObject(dir, LAYER::BACKGROUND);
}

void Stage2::Init()
{
	Scene::Init();
	GET_SINGLE(EndingManager)->Init();

	CollisionManager* cm = GET_SINGLE(CollisionManager);
	ResourceManager* rm = GET_SINGLE(ResourceManager);
	rm->LoadSound(L"BGM", L"Sound\\BGM.mp3", true);
	rm->Play(L"BGM");
	cm->CheckReset();
	cm->CheckLayer(LAYER::TRAP, LAYER::PLAYER);
	cm->CheckLayer(LAYER::PROJECTILE, LAYER::PLAYER);
	cm->CheckLayer(LAYER::BUTTON, LAYER::PLAYER);
	cm->CheckLayer(LAYER::BACKGROUND, LAYER::PLAYER);

	RollingSkillUI* rollingSkill = new RollingSkillUI;
	AddObject(rollingSkill, LAYER::UI);

	Agent* agent = new Agent;
	agent->GetTransform()->SetPosition({ SCREEN_WIDTH / 2 , SCREEN_HEIGHT / 2 });
	AddObject(agent, LAYER::PLAYER);

	Vec2 sp = { SCREEN_WIDTH / 2 , SCREEN_HEIGHT / 2 };
	Vec2 tsp = sp;
#pragma region Map
	sp -= {128, -128};
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	upm(sp, 2);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	upm(sp, 2);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	moveSquareRight(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	moveSquareRight(sp);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 2, 7);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 2);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Down");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 2);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);

	move(sp, 2, 6.5f);
	CreateArrow(sp, L"Right");
	downm(sp, 6.5f);
	CreateUpDown(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);

	moveRight(sp);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);

	move(sp, 2, 7);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 1, 7);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);

	move(sp, 2, 13);
	CreateUpDown(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 5);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 1, 7);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);

	move(sp, 2, 7);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 1, 7);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);

	move(sp, 2, 13);
	CreateUpDown(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 5);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 1, 7);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 5);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);


	move(sp, 3, 13);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);

	moveSquareRight(sp);
	right(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	moveSquareRight(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE);

	move(sp, 2, 5);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP, OBJECT_TYPE::TRAP);

	move(sp, 2, 5);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateUpDown(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE);

	move(sp, 1, 9);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 2, 9);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 2, 9);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP);

	move(sp, 2, 9);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateSquare(sp, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 2, 9);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	upm(sp, 3);
	CreateUpDown(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP);

	move(sp, 1, 9);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP);
	up(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TRAP, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

	move(sp, 3, 5);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);

	move(sp, 3, 1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);
	move(sp, 0.5f, -1);
	CreateArrow(sp, L"Up");
	move(sp, -0.5f, -1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);

	move(sp, 3, 1);
	CreateSquare(sp, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL, OBJECT_TYPE::FALL);

	move(sp, 3, 1);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	moveSquareRight(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);
	moveSquareRight(sp);
	CreateSquare(sp, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE, OBJECT_TYPE::TILE);

#pragma endregion

	move(tsp, 4.5f, -1);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);
	upm(tsp, 12);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);
	move(tsp, 4, 8.5f);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);
	downm(tsp, -3);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);
	move(tsp, 0.5f, -6.5f);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	rightm(tsp, 6);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	downm(tsp, 8);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	leftm(tsp, 4);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	rightm(tsp, 8);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	move(tsp, 2, -8);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	move(tsp, 3, 15);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);
	move(tsp, 3, -17);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);
	move(tsp, 2, 6);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	move(tsp, 3, 2);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);
	move(tsp, 4, -4);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	rightm(tsp, 5);
	CreateObject(tsp, OBJECT_TYPE::TOWER);
	move(tsp, 8, 2);
	CreateObject(tsp, OBJECT_TYPE::FOLLOW);

	move(tsp, 10, -14);
	CreateObject(tsp, OBJECT_TYPE::GOLD);


}

void Stage2::Render(HDC _hdc)
{
	Scene::Render(_hdc);
	GET_SINGLE(UIManager)->RenderHP(_hdc);
}

void Stage2::Release()
{
	Scene::Release();
	ResourceManager* rm = GET_SINGLE(ResourceManager);
	rm->Stop(SOUND_CHANNEL::BGM);
}

Object* Stage2::CreateObject(Vec2 vec, OBJECT_TYPE type)
{
	Object* tile = nullptr;
	switch (type)
	{
	case OBJECT_TYPE::TILE:
		tile = new TileObject;
		AddObject(tile, LAYER::BACKGROUND);
		break;
	case OBJECT_TYPE::TRAP:
		tile = new TrapTileObject;
		AddObject(tile, LAYER::TRAP);
		break;
	case OBJECT_TYPE::FALL:
		tile = new FallTileObject;
		AddObject(tile, LAYER::TRAP);
		break;
	case OBJECT_TYPE::BUTTON:
		tile = new Button;
		AddObject(tile, LAYER::BUTTON);
		break;
	case OBJECT_TYPE::CONDITIONTILE:
		tile = new ConditionTile();
		AddObject(tile, LAYER::BACKGROUND);
		break;
	case OBJECT_TYPE::TOWER:
		tile = new MagicTower(1);
		AddObject(tile, LAYER::TRAP);
		break;
	case OBJECT_TYPE::LAZER:
		tile = new StarLazer(vec);
		AddObject(tile, LAYER::TRAP);
		break;
	case OBJECT_TYPE::FOLLOW:
		tile = new FollowTrap;
		AddObject(tile, LAYER::PROJECTILE);
		break;
	case OBJECT_TYPE::GOLD:
		tile = new Gold;
		AddObject(tile, LAYER::BACKGROUND);
		break;
	default:
		break;
	}

	tile->GetTransform()->SetPosition(vec);

	return tile;
}