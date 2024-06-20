using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor.PackageManager.Requests;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class LSystemTree : MonoBehaviour
{

    //exnter expressions as symbol=mapping
    //no spaces next to equal sign
    [SerializeField] private string[] expressionStrings;
    [SerializeField] private Material[] materials;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private float scalefactor;
    [SerializeField] private GameObject[] leaves;
    public Dictionary<string, string> expressions = new Dictionary<string, string>();
    public GameObject branchPrefab;
    public int numIterations;
    private Transform tempParent;
    public float RotateAngle;
    public float branchStartingLength;
    public float branchStartingwidth;
    public string startSymbol;
    private string generatedGrammar;
    private List<string> parsedGrammar;

    private List<Transform> branches;
    private List<Transform> leavesList;

    public bool isGenerationComplete { get; private set; } = false;

    void Awake()
    {
        branches = new List<Transform>();
        leavesList = new List<Transform>();
        if (expressionStrings.Length == 0)
        {
            Debug.Log("enter expressions");
        }
        else
        {
            populateDict();
            StartCoroutine(GenerateTreeCoroutine());
            
        }
    }

    private IEnumerator GenerateTreeCoroutine()
    {
        generateString();
        parsedGrammar = ParseString(generatedGrammar);
        generateTree();
        yield return null;
        isGenerationComplete = true;
    }

    private void populateDict()
    {
        foreach (string exp in expressionStrings)
        {
            string[] keyValue = exp.Split("=");
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                string value = keyValue[1].Trim();
                expressions[key] = value;
            }
        }
    }

    private void printDict()
    {
        foreach (var key in expressions)
        {
            Debug.Log(key.Key + ": " + key.Value);
        }
    }

    private void printParsed()
    {
        foreach (var str in parsedGrammar)
        {
            Debug.Log(str);
        }
    }
    
    private void generateString()
    {
        string temp;
        generatedGrammar += startSymbol;
        for (int i = 0; i < numIterations; i++)
        {
            temp = "";
            for (int j  = 0; j < generatedGrammar.Length; j++)
            {
                string symbol = generatedGrammar[j].ToString();
                if (expressions.ContainsKey(symbol))
                {
                    temp += expressions[symbol];
                }
                else
                {
                    temp += symbol;
                }
                //Debug.Log(i + " " + symbol + " " + temp);
            }
            //Debug.Log(temp);
            generatedGrammar = temp;
        }
    }
    
    private List<string> ParseString(string input)
    {
        string pattern = @"\+[a-zA-Z]|-[a-zA-Z]|\*[a-zA-Z]|\&[a-zA-Z]|[a-zA-Z\+\-\*\&\[\]]";
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(input);
        
        // Convert matches to a list of strings
        List<string> tokens = new List<string>();
        foreach (Match match in matches)
        {
            tokens.Add(match.Value);
        }
        return tokens;
    }

    public GameObject createBranch(float scale, Vector3 position, Quaternion rotation)
    {
        float branchScaleFactor = scale * branchStartingLength;
        float widthScaleFactor = scale * branchStartingwidth;
        GameObject branch = Instantiate(branchPrefab) as GameObject;
        // branch.transform.parent = parentTransform;
        branch.transform.position = position;
        branch.transform.rotation = rotation;
        branch.transform.localScale = new Vector3(widthScaleFactor, branchScaleFactor, widthScaleFactor);
        branches.Add(branch.transform);

        int leafChoice = Random.Range(0, leaves.Length - 1);
        GameObject leaf = Instantiate(leaves[leafChoice]);
        position  += branch.transform.up * (branch.transform.localScale.y/2 + branchScaleFactor/2);
        leaf.transform.position = position;
        leaf.transform.rotation = rotation;
        leaf.transform.localScale = scale * leaf.transform.localScale;
        leavesList.Add(leaf.transform);
        return branch;
    }

    public void generateTree()
    {
        Stack<GameObject> branchStack = new Stack<GameObject>();
        int depth = 0;
        Vector3 currPosition = Vector3.zero;
        Quaternion currRotation = Quaternion.identity;
        GameObject obj = Instantiate(branchPrefab) as GameObject;
        obj.transform.parent = parentTransform;
        obj.transform.localPosition = currPosition;
        obj.transform.localRotation = currRotation;
        obj.transform.localScale = new Vector3(branchStartingwidth, branchStartingLength, branchStartingwidth);
        float scale = 1f;
        int level = 1;

        foreach (var val in parsedGrammar)
        {
            if (val == "[")
            {
                branchStack.Push(obj);
                //at each level reduce scale by factor of 0.8

                scale *= scalefactor;
                level += 1;
            }
            else if (val == "]")
            {
                GameObject prev = branchStack.Pop();
                
                currPosition = prev.transform.position;
                currRotation = prev.transform.rotation;
                obj = prev;
                scale /= scalefactor;
                level -= 1;
            } 
            else if (val.Length == 1 )
            {
                //obj is currently previous branch
                float branchScaleFactor = scale * branchStartingLength;
                currPosition  += obj.transform.up * (obj.transform.localScale.y/2 + branchScaleFactor/2);
                obj  = createBranch(scale, currPosition, currRotation);
            }
            else
            {
                float xangle;
                float yangle;
                float zangle;
                if (val[0] == '-')
                {
                    xangle = -RotateAngle + Random.Range(-10,10);
                    zangle = Random.Range(-80,-10)*scalefactor;
                }
                else if (val[0] == '+')
                {
                    xangle = RotateAngle +  Random.Range(-10,10);
                    zangle = Random.Range(10, 80)*scalefactor;
                }
                
                else if (val[0] == '*')
                {
                    xangle = RotateAngle + Random.Range(-10,10);
                    zangle = Random.Range(-170,-100)*scalefactor;

                }
                
                else
                {
                    xangle = -RotateAngle + Random.Range(-10,10);
                    zangle = Random.Range(100, 170)*scalefactor;
                }

                yangle = Random.Range(0, 90);
                float branchScaleFactor = scale * branchStartingLength;
                //at each level reduce scale by factor of 0.8
                currPosition  += obj.transform.up * (obj.transform.localScale.y/2 + branchScaleFactor/2);
                Quaternion additionalRotationQuat = Quaternion.Euler(xangle, yangle, zangle);
                currRotation *= additionalRotationQuat;
                obj = createBranch(scale, currPosition, currRotation);
            }
        }

        foreach (var trfm in branches)
        {
            trfm.parent = parentTransform;
        }

        foreach (var trfm in leavesList)
        {
            trfm.parent = parentTransform;
        }
    }
    void Start()
    {
        
    }
}
