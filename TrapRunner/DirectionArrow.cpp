#include "pch.h"
#include "DirectionArrow.h"
#include "SpriteRenderer.h"
#include "InputManager.h"
#include "SceneManager.h"

DirectionArrow::DirectionArrow()
{
	GetTransform()->SetScale({ 32,32 });

	AddComponent<SpriteRenderer>();
	SpriteRenderer* spriteRenderer = GetComponent<SpriteRenderer>();
	spriteRenderer->CreateTexture(L"Texture\\SelectArrow.bmp", L"selectArrow");
	GetTransform()->SetPosition({ 100 , 300 });
}

DirectionArrow::~DirectionArrow()
{
}

void DirectionArrow::ChangeTypeToExit() {
	selectType = SELECT_TYPE::EXIT;
	GetTransform()->SetPosition({ 100, exitPos });
}

void DirectionArrow::ChangeTypeToPlay() {
	selectType = SELECT_TYPE::PLAY;
	GetTransform()->SetPosition({ 100, playPos });
}

void DirectionArrow::Update()
{
	if (GET_KEYDOWN(KEY_TYPE::S) || GET_KEYDOWN(KEY_TYPE::W)) {
		if (selectType == SELECT_TYPE::PLAY)
			ChangeTypeToExit();
		else ChangeTypeToPlay();
	}
	if (GET_KEYDOWN(KEY_TYPE::SPACE)) {
		switch (selectType)
		{
		case SELECT_TYPE::PLAY:
			GET_SINGLE(SceneManager)->LoadScene(L"StageSelectScene");
			break;
		case SELECT_TYPE::EXIT:
			HANDLE hProcess = GetCurrentProcess();
			TerminateProcess(hProcess, 0);
			break;
		}
	}
}

void DirectionArrow::Render(HDC _hdc)
{
	ComponentRender(_hdc);
}

void DirectionArrow::EnterCollision(Collider* _other)
{
}

void DirectionArrow::StayCollision(Collider* _other)
{
}

void DirectionArrow::ExitCollision(Collider* _other)
{
}
