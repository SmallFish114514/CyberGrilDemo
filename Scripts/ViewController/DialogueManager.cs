using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public DialgueContent[] DialgueContents = new DialgueContent[11]
	{
		new DialgueContent
		{
			isLeft = false,
			Sentence = "你好"
		},
		new DialgueContent
		{
			isLeft = true,
			Sentence = "你是？"
		},
		new DialgueContent
		{
			isLeft = false,
			Sentence = "我是这里的NPC"
		},
		new DialgueContent
		{
			isLeft = false,
			Sentence = "欢迎来到中城"
		},
		new DialgueContent
		{
			isLeft = false,
			Sentence = "你的任务是清理至少13只安保即可去往地图最右方通往下一目标点"
		},
		new DialgueContent
		{
			isLeft = true,
			Sentence = "了解"
		},
		new DialgueContent
		{
			isLeft = false,
			Sentence = "稍后可以尝试尝试移动以及攻击"
		},
		new DialgueContent
		{
			isLeft = false,
			Sentence = "J键为攻击 空格为跳 Tab为背包"
		},
		new DialgueContent
		{
			isLeft = false,
			Sentence = "战斗过程中留意你的生命值和弹药值，击杀敌人可能会掉落补给品注意观察"
		},
		new DialgueContent
		{
			isLeft = false,
			Sentence = "留意地图中散落的武器，它会添加进你的背包，通过数字键进行切换武器"
		},
		new DialgueContent
		{
			isLeft = true,
			Sentence = "了解"
		}
	};

	private Text playerDialogueText;

	private Text npcDialogueText;

	private int currentDialogueIndex;

	private void Awake()
	{
		playerDialogueText = (transform.Find("BG/PlayerDialogueText")).GetComponent<Text>();
		npcDialogueText = (transform.Find("BG/NpcDialogueText")).GetComponent<Text>();
	}

	private void ShowDialogue(string str, Text targetText)
	{
		targetText.text = "";
		for (int i = 0; i < str.Length; i++)
		{
			targetText.text += str[i];
		}
		currentDialogueIndex++;
	}

	public void UpdateDialogue()
	{
		if (currentDialogueIndex < DialgueContents.Length)
		{
			gameObject.SetActive(true);
			if (DialgueContents[currentDialogueIndex].isLeft)
			{
				((playerDialogueText).transform).gameObject.SetActive(true);
				((npcDialogueText).transform).gameObject.SetActive(false);
				ShowDialogue(DialgueContents[currentDialogueIndex].Sentence, playerDialogueText);
			}
			else
			{
				((playerDialogueText).transform).gameObject.SetActive(false);
				((npcDialogueText).transform).gameObject.SetActive(true);
				ShowDialogue(DialgueContents[currentDialogueIndex].Sentence, npcDialogueText);
			}
		}
		else
		{
			gameObject.SetActive(false);
			currentDialogueIndex = 0;
		}
	}
}
