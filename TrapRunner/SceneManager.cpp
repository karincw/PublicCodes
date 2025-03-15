#include "pch.h"
#include "SceneManager.h"
#include "Scene.h"
#include "PigScene.h"
#include "Title.h"
#include "BishojoScene.h"
#include "TimeManager.h"
#include "Stage2.h"
#include "StageSelectScene.h"
#include "EndingScene.h"
#include "CollisionManager.h"
void SceneManager::Init()
{
	m_pCurrentScene = nullptr;

	// ¾À µî·Ï
	RegisterScene(L"BishojoScene",std::make_shared<BishojoScene>());
	RegisterScene(L"Title",std::make_shared<Title>());
	RegisterScene(L"PigScene",std::make_shared<PigScene>());
	RegisterScene(L"StageSelectScene",std::make_shared<StageSelectScene>());
	RegisterScene(L"Stage2",std::make_shared<Stage2>());
	RegisterScene(L"EndingScene",std::make_shared<EndingScene>());

	// ¾À ·Îµå
	LoadScene(L"Title");
}

void SceneManager::Update()
{
	if (m_pCurrentScene == nullptr)
		return;
	m_pCurrentScene->Update();
	m_pCurrentScene->LateUpdate();
}

void SceneManager::Render(HDC _hdc)
{
	if (m_pCurrentScene == nullptr)
		return;
	m_pCurrentScene->Render(_hdc);
}

void SceneManager::RegisterScene(const wstring& _sceneName, std::shared_ptr<Scene> _scene)
{
	if (_sceneName.empty() || _scene == nullptr)
		return;
	m_mapScenes.insert(m_mapScenes.end(), {_sceneName, _scene});
}

void SceneManager::LoadScene(const wstring& _sceneName)
{
	GET_SINGLE(CollisionManager)->CheckReset();
	// ¾ÀÀÌ ÀÖÀ¸¸é
	if (m_pCurrentScene != nullptr)
	{
		m_pCurrentScene->Release();
		m_pCurrentScene = nullptr;
		GET_SINGLE(TimeManager)->ReleaseDelays();
	}

	auto iter = m_mapScenes.find(_sceneName);
	if (iter != m_mapScenes.end())
	{
		m_pCurrentScene = iter->second;
		m_pCurrentScene->Init();
	}
}

void SceneManager::SafeLoadScene(const wstring& _sceneName)
{
	GET_SINGLE(CollisionManager)->SafeLoadScene(_sceneName);
}
