#include "pch.h"
#include "Projectile.h"
#include "TimeManager.h"
#include "Texture.h"
#include "ResourceManager.h"
#include "Collider.h"
#include "EventManager.h"
#include "SceneManager.h"
#include "Scene.h"
#include "Camera.h"
#include "SpriteRenderer.h"

Projectile::Projectile()
	: m_angle(0.f)
	, m_speed(0.f)
	, m_vDir(1.f, 1.f)
{
	AddComponent<SpriteRenderer>();
	AddComponent<Collider>();
}

Projectile::~Projectile()
{
}

void Projectile::Update()
{
	Vec2 vPos = { 0,0 };

	vPos.x = m_vDir.x * m_speed * fDT;
	vPos.y = m_vDir.y * m_speed * fDT;
	Transform* trm = GetTransform();
	trm->Translate(vPos);

	auto currentScene = GET_SINGLE(SceneManager)->GetCurrentScene();
	if (currentScene == nullptr || currentScene->GetCamera() == nullptr)
		return;

	Vec2 CamPos = currentScene->GetCamera()->GetWorldPosition();
	bool State = true;
	Vec2 LeftTop = { CamPos.x - SCREEN_WIDTH / 2, CamPos.y - SCREEN_WIDTH / 2 };
	Vec2 RightBottom = { CamPos.x + SCREEN_WIDTH / 2, CamPos.y + SCREEN_HEIGHT / 2 };
	Vec2 position = trm->GetPosition();
	Vec2 scale = trm->GetScale();

	if (position.x + SCREEN_WIDTH < LeftTop.x)
		State = false;
	else if (position.x - SCREEN_WIDTH > RightBottom.x)
		State = false;
	else if (position.y + SCREEN_HEIGHT < LeftTop.y)
		State = false;
	else if (position.y - SCREEN_HEIGHT > RightBottom.y)
		State = false;

	if (!State)
		GET_SINGLE(EventManager)->DeleteObject(this);

}

void Projectile::Render(HDC _hdc)
{
	ComponentRender(_hdc);
}
