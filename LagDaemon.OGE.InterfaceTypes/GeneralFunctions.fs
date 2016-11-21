namespace LagDaemon.OGE.InterfaceTypes

  module LibFuncs =
  
    let hook observer f param =
      let y = f param
      observer y
      y
     
    let hook2 observer f param1 param2 =
      let y = f param1 param2
      observer y
      y
    
    let hook3 observer f param1 param2 param3 =
      let y = f param1 param2 param3
      observer y
      y
      
        
    
    
