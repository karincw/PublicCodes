#include "pch.h"
#include "TileObject.h"
#include "SpriteRenderer.h"
#include "Transform.h"
#include "Collider.h"
#include "Agent.h"

TileObject::TileObject()
{
	GetTransform()->SetScale({ 256,256 });
	AddComponent<SpriteRenderer>();
	GetComponent<SpriteRenderer>()->CreateTexture(L"Texture\\tile.bmp", L"tile");

	AddComponent<Collider>();
	Collider* col = GetComponent<Collider>();
	col->SetSize({ 256, 256 });

	SetName(L"Tile");
}

TileObject::~TileObject()
{
}

void TileObject::Update()
{
}

void TileObject::Render(HDC _hdc)
{
	ComponentRender(_hdc);
}

void TileObject::EnterCollision(Collider* _other)
{
}

void TileObject::StayCollision(Collider* _other)
{
	Object* obj = _other->GetOwner();
	if (obj->GetName() == L"Player")
	{
		Agent* pObj = dynamic_cast<Agent*>(obj);
		pObj->backUpTile = this;
	}
}

void TileObject::ExitCollision(Collider* _other)
{
}
