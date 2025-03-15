#pragma once
#include "Object.h"
class Lazer;

class StarLazer : public Object
{
public:
	StarLazer(Vec2 position);
	~StarLazer();
public:
	void Update()                         override;
	void Render(HDC _hdc)                 override;
	void EnterCollision(Collider* _other) override;
	void StayCollision(Collider* _other)  override;
	void ExitCollision(Collider* _other)  override;
public:
	void OpenHorizontal();
	void OpenVertical();
	void CloseLazer();
	bool isHorizontal = true;
	wstring idx = L"2";
private:
	Lazer* lazers[4];
};

