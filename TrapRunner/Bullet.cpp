#include "pch.h"
#include "Bullet.h"
#include "Collider.h"
#include "SpriteRenderer.h"

Bullet::Bullet()
{
	GetTransform()->SetScale(Vec2(40, 40));
	GetComponent<SpriteRenderer>()->CreateTexture(L"Texture\\Bullet_Red.bmp", L"Bullet_Red");

	GetComponent<Collider>()->SetSize(Vec2(32, 32));
	SetName(L"Bullet");
}

Bullet::~Bullet()
{
}

void Bullet::Update()
{
	Projectile::Update();
}

void Bullet::Render(HDC _hdc)
{
	ComponentRender(_hdc);
}

void Bullet::EnterCollision(Collider* _other)
{

}

void Bullet::StayCollision(Collider* _other)
{
}

void Bullet::ExitCollision(Collider* _other)
{
}
