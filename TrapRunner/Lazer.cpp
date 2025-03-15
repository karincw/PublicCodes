#include "pch.h"
#include "Lazer.h"
#include "SpriteRenderer.h"
#include "Collider.h"
#include "Agent.h"

std::wstring toString(LAZER_DIR dir)
{
	switch (dir)
	{
	case LAZER_DIR::Left:
		return L"Left";
	case LAZER_DIR::Right:
		return L"Right";
	case LAZER_DIR::Top:
		return L"Top";
	case LAZER_DIR::Bottom:
		return L"Bottom";
	}
	return nullptr;
}

Vec2 GetColliderPos(LAZER_DIR dir, Vec2 Scale)
{
	switch (dir)
	{
	case LAZER_DIR::Left:
		return Vec2(-Scale.x / 4, 0.0f);
	case LAZER_DIR::Right:
		return Vec2(Scale.x / 4, 0.0f);
	case LAZER_DIR::Top:
		return Vec2(0.0f, -Scale.y / 4);
	case LAZER_DIR::Bottom:
		return Vec2(0.0f, Scale.y / 4);
	}
}
Vec2 GetColliderScale(LAZER_DIR dir, Vec2 Scale)
{
	switch (dir)
	{
	case LAZER_DIR::Left:
	case LAZER_DIR::Right:
		return Vec2(Scale.x / 2, 20.0f);
	case LAZER_DIR::Top:
	case LAZER_DIR::Bottom:
		return Vec2(20.0f, Scale.y / 2);
	}
}

Lazer::Lazer(LAZER_DIR dir, Vec2 Scale)
	: _enable(false)
{
	GetTransform()->SetScale(Scale);

	AddComponent<SpriteRenderer>();
	SpriteRenderer* renderer = GetComponent<SpriteRenderer>();
	renderer->CreateTexture(L"Texture\\Lazer_" + toString(dir) + L".bmp", L"Lazer_" + toString(dir));

	AddComponent<Collider>();
	Collider* col = GetComponent<Collider>();
	col->SetOffSetPos(GetColliderPos(dir, Scale));
	col->SetSize(GetColliderScale(dir, Scale));

	SetName(L"Lazer");
	SetEnable(_enable);
}

Lazer::~Lazer()
{
}

void Lazer::Update()
{
}

void Lazer::Render(HDC _hdc)
{
	ComponentRender(_hdc);
}

void Lazer::StayCollision(Collider* _other)
{
	Object* obj = _other->GetOwner();
	if (obj->GetName() == L"Player")
	{
		Agent* agent = dynamic_cast<Agent*>(obj);
		agent->Hit();
	}
}

void Lazer::SetEnable(bool state)
{
	_enable = state;
	GetComponent<Collider>()->SetEnable(state);
	GetComponent<SpriteRenderer>()->SetEnable(state);
}
