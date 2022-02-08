using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TokenGridManager : MonoBehaviour
{
       public GameObject tokenDisplayPrefab;
    ERC721Metadata[] tokensList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadTokens(ERC721Metadata[] tokens)
    {
        tokensList = tokens;
        foreach(var token in tokensList)
        {
            var instance = Instantiate<GameObject>(tokenDisplayPrefab, transform);
            instance.GetComponent<TokenDisplay>().UpdateTokenMetadata(token);
        }
    }
}
