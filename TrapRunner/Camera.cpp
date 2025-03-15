#include "pch.h"
#include "Camera.h"
#include "Scene.h"


Camera::Camera()
	:currentScene(nullptr)
{

}

Camera::~Camera()
{

}

void Camera::Update()
{

}

void Camera::Render(HDC _hdc)
{

}

void Camera::SetScene(Scene* scene)
{
	currentScene = scene;
}