using System;
using RedBjorn.ProtoTiles;
using UnityEngine;

public class BattleField : MonoBehaviour
{

    public Action OnFieldReady;
    public MapSettings Field;
    public MapEntity FieldEntity {get; private set; }
    private MapView _mapView;
    
    public void InitField(MapSettings field)
    {
        if (_mapView != null)
            Destroy(_mapView);

        Field = field;
        
        _mapView = Instantiate(Field.MapViewPrefab,transform);
        FieldEntity = new MapEntity(Field,_mapView );
        _mapView.Init(FieldEntity);


        OnFieldReady?.Invoke();
        Debug.Log($"Setup Field - {field.name} ");
    }

    [ContextMenu("Test field setup")]
    public void TestInitField()
    {
        InitField(Field);
    }
}