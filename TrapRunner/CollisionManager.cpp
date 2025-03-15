#include "pch.h"
#include "CollisionManager.h"
#include "SceneManager.h"
#include "Scene.h"
#include "Object.h"
#include "Collider.h"
#include "CircleCollider.h"
void CollisionManager::Update()
{
	if (!GET_SINGLE(SceneManager)->GetCurrentScene())
		return;

	for (UINT Row = 0; Row < (UINT)LAYER::END; ++Row)
	{
		for (UINT Col = Row; Col < (UINT)LAYER::END; ++Col)
		{
			if (m_arrLayer[Row] & (1 << Col))
			{
				//int a = 0;
				CollisionLayerUpdate((LAYER)Row, (LAYER)Col);
			}
		}
	}
}

void CollisionManager::CheckLayer(LAYER _left, LAYER _right)
{
	// 작은쪽을 행으로 씁시다.
	// 작은 값을 LAYER의 행으로, 큰 값을 열
	UINT Row = (UINT)_left;
	UINT Col = (UINT)_right;
	//Row = min(Row, Col);
	if (Row > Col)
		std::swap(Row, Col);
	//m_arrLayer[Row];
	//Col;

	//// 비트 연산
	// 체크가 되어있다면
	if (m_arrLayer[Row] & (1 << Col))
	{
		// 체크 풀기
		m_arrLayer[Row] &= ~(1 << Col);
	}
	// 체크가 안되어있다면r
	else
	{
		m_arrLayer[Row] |= (1 << Col);
	}
	int a = 0;
}

void CollisionManager::CheckReset()
{
	// 메모리 초기화
	memset(m_arrLayer, 0, sizeof(UINT) * (UINT)LAYER::END);
}
void CollisionManager::CollisionLayerUpdate(LAYER _left, LAYER _right)
{
	std::shared_ptr<Scene> pCurrentScene = GET_SINGLE(SceneManager)->GetCurrentScene();
	const vector<Object*>& vecLeftLayer = pCurrentScene->GetLayerObjects(_left);
	const vector<Object*>& vecRightLayer = pCurrentScene->GetLayerObjects(_right);
	map<ULONGLONG, bool>::iterator iter;
	for (size_t i = 0; i < vecLeftLayer.size(); ++i)
	{
		std::vector<Collider*> pLeftColliders = vecLeftLayer[i]->GetComponents<Collider>();
		// 충돌체 없는 경우
		if (pLeftColliders.empty())
			continue;

		for (size_t j = 0; j < vecRightLayer.size(); j++)
		{
			std::vector<Collider*> pRightColliders = vecRightLayer[j]->GetComponents<Collider>();
			// 충돌체가 없거나, 자기자신과의 충돌인 경우
			if (pRightColliders.empty() || vecLeftLayer[i] == vecRightLayer[j])
				continue;

			COLLIDER_ID colliderID; // 두 충돌체로만 만들 수 있는 ID

			for (auto leftCol : pLeftColliders)
			{
				if (!leftCol->GetEnable() || !leftCol) continue;
				for (auto rightCol : pRightColliders)
				{
					if (!rightCol->GetEnable() || !rightCol) continue;
					colliderID.left_ID = leftCol->GetID();
					colliderID.right_ID = rightCol->GetID();

					iter = m_mapCollisionInfo.find(colliderID.ID);
					// 이전 프레임 충돌한 적 없다.
					if (iter == m_mapCollisionInfo.end())
					{
						// 충돌 정보가 미등록된 상태인 경우 등록(충돌하지 않았다로)
						m_mapCollisionInfo.insert({ colliderID.ID, false });
						//m_mapCollisionInfo[colliderID.ID] = false;
						iter = m_mapCollisionInfo.find(colliderID.ID);
					}

					if (IsCollision(leftCol, rightCol))
					{
						// 이전에도 충돌중
						if (iter->second)
						{
							if (vecLeftLayer[i]->GetIsDead() || vecRightLayer[j]->GetIsDead())
							{
								leftCol->ExitCollision(rightCol);
								rightCol->ExitCollision(leftCol);
								iter->second = false;
							}
							else
							{
								leftCol->StayCollision(rightCol);
								rightCol->StayCollision(leftCol);
							}
						}
						else // 이전에 충돌 x
						{
							if (!vecLeftLayer[i]->GetIsDead() && !vecRightLayer[j]->GetIsDead())
							{
								leftCol->EnterCollision(rightCol);
								rightCol->EnterCollision(leftCol);
								iter->second = true;
							}
						}
					}
					else // 충돌 안하네?
					{
						if (iter->second) // 근데 이전에 충돌중
						{
							leftCol->ExitCollision(rightCol);
							rightCol->ExitCollision(leftCol);
							iter->second = false;
						}
					}
				}
			}
		}
	}
	if (loadScene)
	{
		loadScene = false;
		GET_SINGLE(SceneManager)->LoadScene(sceneName);
	}

}

bool CollisionManager::IsCollision(Collider* _left, Collider* _right)
{
	CircleCollider* circleLeft = dynamic_cast<CircleCollider*>(_left);
	CircleCollider* circleRight = dynamic_cast<CircleCollider*>(_right);

	if (circleLeft != nullptr && circleRight != nullptr) // 둘다 원
	{
		float maxDist = max(circleLeft->GetRadius(), circleRight->GetRadius());
		auto leftPos = circleLeft->GetOwner()->GetTransform()->GetPosition();
		auto rightPos = circleRight->GetOwner()->GetTransform()->GetPosition();
		auto len = leftPos - rightPos;
		if (maxDist >= len.Length())
		{
			return true;
		}
	}
	else if (circleLeft != nullptr || circleRight != nullptr) // 하나만 원
	{
		bool leftIsCircle = circleLeft != nullptr ? true : false;
		Object* square = leftIsCircle ? _right->GetOwner() : _left->GetOwner();
		Object* circle = leftIsCircle ? _left->GetOwner() : _right->GetOwner();

		Vec2 rectPos = square->GetTransform()->GetPosition() + square->GetComponent<Collider>()->GetOffSetPos();
		Vec2 rectSize = square->GetComponent<Collider>()->GetSize();
		Vec2 circlePos = circle->GetTransform()->GetPosition() + circle->GetComponent<CircleCollider>()->GetOffSetPos();
		float circleRad = circleLeft->GetRadius();

		// 2. 사각형의 경계를 구해올겁니다.
		float left = rectPos.x - (rectSize.x / 2);
		float right = rectPos.x + (rectSize.x / 2);
		float top = rectPos.y - (rectSize.y / 2);
		float bottom = rectPos.y + (rectSize.y / 2);

		// 충돌체크를 위한 가장가까운 값을 가져올거야.
		float closestX = std::clamp(circlePos.x, left, right);
		float closestY = std::clamp(circlePos.y, top, bottom);

		float dx = circlePos.x - closestX;
		float dy = circlePos.y - closestY;
		float dist = sqrt(dx * dx + dy * dy);

		if (dist <= circleRad)
			return true;
	}
	else //둘다 네모
	{
		Vec2 vLeftPos = _left->GetLatedUpatedPos();
		Vec2 vRightPos = _right->GetLatedUpatedPos();
		Vec2 vLeftSize = _left->GetSize();
		Vec2 vRightSize = _right->GetSize();

		RECT leftRt = RECT_MAKE(vLeftPos.x, vLeftPos.y, vLeftSize.x, vLeftSize.y);
		RECT rightRt = RECT_MAKE(vRightPos.x, vRightPos.y, vRightSize.x, vRightSize.y);
		RECT rt;
		return ::IntersectRect(&rt, &leftRt, &rightRt);
	}
}

void CollisionManager::SafeLoadScene(wstring s)
{
	sceneName = s;
	loadScene = true;
}
