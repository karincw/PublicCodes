#include "pch.h"
#include "Direction.h"
#include "SpriteRenderer.h"

Direction::Direction(wstring dir)
{
	GetTransform()->SetScale({ 128,128 });
	AddComponent<SpriteRenderer>();
	SpriteRenderer* sp = GetComponent<SpriteRenderer>();

	sp->CreateTexture(L"Texture\\DirectionArrow_" + dir + L".bmp", L"Direction" + dir);
}

Direction::~Direction()
{
}

void Direction::Update()
{
}

void Direction::Render(HDC _hdc)
{
	ComponentRender(_hdc);
}
