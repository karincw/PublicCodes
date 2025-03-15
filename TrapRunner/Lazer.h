#pragma once
#include "Object.h"
enum class LAZER_DIR
{
	Left,
	Right,
	Top,
	Bottom,
};

class Lazer : public Object
{
public:
	Lazer(LAZER_DIR dir, Vec2 Scale);
	~Lazer();
public:
	void Update() override;
	void Render(HDC _hdc) override;
	void StayCollision(Collider* _other) override;
	void SetEnable(bool state);
private:
	bool _enable;
};

