using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public LevelManager levelManager;
    [SerializeField] TextMeshProUGUI budgetText;
    void Start()
    {
        //SetBudgetText();
    }
    public bool SetBudget(int c)
    {
        bool a = levelManager.SetBudget(c);
        if(a)
        {
            SetBudgetText();
            return true;
        }
        else
        {
            Debug.Log("Not enough resources");
            return false;
        }
    }
    void SetBudgetText()
    {
        budgetText.text = levelManager.budget.ToString();
    }
}
