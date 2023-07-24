using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAccount
{

    private int accountNum;//유저 넘버
    private string userName;//유저 이름
    private string email;//유저 이름
    private int characterCount;//보유 캐릭터
    private int houseCount;//보유 은신처
    private int studyCount;//보유 학습
    private int trainingCount;//보유 단련

    public DefaultAccount(int accNum, string userNameTyping, string userEmail, int c, int h, int s, int t)
    {
        this.accountNum = accNum;
        this.userName = userNameTyping;
        this.email = userEmail;
        this.characterCount = c;//보유 캐릭터
        this.houseCount = h;//보유 은신처
        this.studyCount = s;//보유 학습
        this.trainingCount = t; //보유 단련
    }

    /*public int SetAccountNumber(int userNumber)
    {
        if (userNumber==0)
        {
            return 1;
        }
        else
        {
            accountNum = userNumber;
            return 0;
        }

    }

    public int SetUserName(string userNameTyping)
    {
        if (userNameTyping == null)
        {
            return 1;
        }
        else
        {
            userName = userNameTyping;
            return 0;
        }
    }
    public int SetCharacterCount(int charaterNumber)
    {
        if (charaterNumber == 0)
        {
            return 1;
        }
        else
        {
            characterCount.Add(charaterNumber);//캐릭터가 서버에 저장되면 저장된 캐릭터 번호를 계정에 추가한다.
            return 0;
        }
    }

    public int SetHouseCount(int houseNumber)
    {
        if (houseNumber == 0)
        {
            return 1;
        }
        else
        {
            houseCount.Add(houseNumber);
            return 0;
        }
    }

    public int SetStudyCount(int studyNumber)
    {
        if (studyNumber == 0)
        {
            return 1;
        }
        else
        {
            studyCount.Add(studyNumber);
            return 0;
        }
    }

    public int SetTrainingCount(int trainingNumber)
    {
        if (trainingNumber == 0)
        {
            return 1;
        }
        else
        {
            trainingCount.Add(trainingNumber);
            return 0;
        }
    }

    public int GetAccountNumber()
    {
        if (accountNum == 0)
        {
            return 1;
        }
        else
        {
            return accountNum;
        }
    }

    public string GetUserName()
    {
        if (userName == null)
        {
            return null;
        }
        else
        {
            return userName;
        }
    }
    public List<int> GetCharacterCount()
    {
        List<int>charaterResult = new List<int>();
        if (characterCount.Count== 0)
        {
            return charaterResult;
        }
        else
        {
            for(int i=0; i<characterCount.Count;i++)
            {
                charaterResult.Add(characterCount[i]);
            }
            return charaterResult;
        }
    }

    public List<int> GetHouseCount(int houseNumber)
    {
        List<int> houseResult = new List<int>();
        if (houseCount.Count == 0)
        {
            return houseResult;
        }
        else
        {
            for (int i = 0; i < houseCount.Count; i++)
            {
                houseResult.Add(houseCount[i]);
            }
            return houseResult;
        }
    }

    public List<int> GetStudyCount()
    {
        List<int> studyResult = new List<int>();
        if (characterCount.Count == 0)
        {
            return studyResult;
        }
        else
        {
            for (int i = 0; i < studyCount.Count; i++)
            {
                studyResult.Add(studyCount[i]);
            }
            return studyResult;
        }
    }

    public List<int> GetTrainingCount()
    {
        List<int> trainingResult = new List<int>();
        if (characterCount.Count == 0)
        {
            return trainingResult;
        }
        else
        {
            for (int i = 0; i < trainingCount.Count; i++)
            {
                trainingResult.Add(trainingCount[i]);
            }
            return trainingResult;
        }
    }*/
}