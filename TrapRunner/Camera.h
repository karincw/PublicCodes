#pragma once
#include "Object.h"
class Scene;
class Camera :
	public Object
{
public:
	Camera();
	~Camera();
	void Update() override;
	void Render(HDC _hdc) override;
public:
	void SetScene(Scene* scene);
	Vec2 GetWorldPosition()
	{
		Vec2 pos = GetTransform()->GetPosition();
		return { pos.x + SCREEN_WIDTH / 2, pos.y + SCREEN_HEIGHT / 2 };
	}
private:
	Scene* currentScene;
};

