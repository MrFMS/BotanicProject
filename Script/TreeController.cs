using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoundPixel;

public class TreeController : MonoSingleton<TreeController>
{
    public List<Tree> trees = new List<Tree>();
    public GameObject TreePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateTree(Vector3 pos)
    {
        GameObject tree = Instantiate(TreePrefab, pos, Quaternion.Euler(0, 0, 0));
        trees.Add(tree.GetComponent<Tree>());
        //Render tree with first generation
    }

    public void DestroyTree(GameObject tree)
    {
        trees.Remove(tree.GetComponent<Tree>());
        Destroy(tree);
    }

    public void GrowingUp(GameObject Tree)
    {
        Debug.Log("laaa");
        Tree.GetComponent<Branch>().tree.GenerateOneIteration();
    }


}
