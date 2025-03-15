#include "pch.h"
#include "FollowTrap.h"
#include "Collider.h"
#include "CircleCollider.h"
#include "Action.h"
#include "TimeManager.h"
#include "SpriteRenderer.h"
#include "Animator.h"
#include "Animation.h"
#include "Agent.h"
#include "EventManager.h"
#include "Explosion.h"
#include "SceneManager.h"
#include "Scene.h"

void DetectColliderEnter(Collider* _other, Object* owner)
{
	Object* obj = _other->GetOwner();
	if (obj->GetName() != L"Player") return;

	FollowTrap* ft = dynamic_cast<FollowTrap*>(owner);
	ft->currentTarget = obj;
	ft->GetComponent<Animator>()->PlayAnimation(L"warning", true);

}
void DetectColliderExit(Collider* _other, Object* owner)
{
	Object* obj = _other->GetOwner();
	if (obj->GetName() != L"Player") return;

	FollowTrap* ft = dynamic_cast<FollowTrap*>(owner);
	ft->currentTarget = nullptr;
	ft->GetComponent<Animator>()->PlayAnimation(L"basic", false);
}
void ColliderStay(Collider* _other, Object* owner)
{
	Object* obj = _other->GetOwner();
	if (obj->GetName() != L"Player") return;
	Agent* ag = dynamic_cast<Agent*>(obj);
	if (!ag->isRolling && ag->canHit)
	{
		Explosion* boom = new Explosion();
		Transform* trm = boom->GetTransform();
		trm->SetPosition(obj->GetTransform()->GetPosition() + Vec2(30, 20));
		GET_SINGLE(SceneManager)->GetCurrentScene()->AddObject(boom, LAYER::PROJECTILE);
		GET_SINGLE(EventManager)->DeleteObject(owner);
		ag->Hit();
	}
}

void FollowTrap::Accel()
{
	if (currentTarget == nullptr)
	{
		Deceleration();
		return;
	}

	Transform* ownerTrm = GetTransform();
	Transform* targetTrm = currentTarget->GetTransform();

	Vec2 targetDir = targetTrm->GetPosition() - ownerTrm->GetPosition();
	targetDir.Normalize();

	Vec2 midDir = (targetDir + moveDir);
	midDir.Normalize();

	moveDir = (targetDir + moveDir * 12);

	if (moveDir.Length() == 0) moveDir = targetDir;

	if ((targetDir - midDir).Length() < MaxAccelableDir)
	{
		Accelation();
	}
	else
	{
		Deceleration();
	}
}

FollowTrap::FollowTrap()
	: speed(0)
	, currentTarget(nullptr)
	, moveDir({ 0,0 })
{
	GetTransform()->SetScale({ 40, 65 });
	Animator* ani = new Animator();
	AddComponent<Animator>(ani);
	ani->CreateTexture(L"Texture\\FollowTrap.bmp", L"FollowTrap_sheet");
	ani->CreateAnimation(L"basic", Vec2(0, 0), Vec2(16, 26), Vec2(0, 0), 1, 0.1f, false);
	ani->CreateAnimation(L"warning", Vec2(0, 0), Vec2(16, 26), Vec2(16, 0), 2, 0.25f, false);

	CircleCollider* col = new CircleCollider();
	CircleCollider* detectCol = new CircleCollider();

	col->collisionStayEvent->Insert(ColliderStay);
	col->SetRadius(32);
	AddComponent<CircleCollider>(col);

	detectCol->collisionEnterEvent->Insert(DetectColliderEnter);
	detectCol->collisionExitEvent->Insert(DetectColliderExit);
	AddComponent<CircleCollider>(detectCol);
	detectCol->SetRadius(1);

	ani->PlayAnimation(L"basic", false);
}

FollowTrap::~FollowTrap()
{
}

void FollowTrap::Update()
{
	Accel();

	if (speed >= maxSpeed)
		speed = maxSpeed;
	else if (speed <= 0)
		speed = 0;

	moveDir.Normalize();
	GetTransform()->Translate(moveDir * speed * fDT);
}

void FollowTrap::Render(HDC _hdc)
{
	ComponentRender(_hdc);
}

void FollowTrap::Accelation()
{
	speed += AccelationSpeed;
}

void FollowTrap::Deceleration()
{
	speed += DecelationSpeed;
}
