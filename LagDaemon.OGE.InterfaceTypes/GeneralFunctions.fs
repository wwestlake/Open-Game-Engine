(*
    Open Game Engine
    Copyright (C) 2016  William W. Westlake
    wwestlake@lagdaemon.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    Source code available at: https://github.com/wwestlake/Open-Game-Engine
*)
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
      
        
    
    
