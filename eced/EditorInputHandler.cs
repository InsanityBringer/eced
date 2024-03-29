﻿/*  ---------------------------------------------------------------------
 *  Copyright (c) 2020 ISB
 *
 *  eced is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *   eced is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with eced.  If not, see <http://www.gnu.org/licenses/>.
 *  -------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eced
{
    //Sometimes I think I'm adding too much separation in this code, but I guess only time will tell
    //This should eventually handle key binding and the like.
    public class EditorInputHandler
    {
        private EditorState state;

        public EditorInputHandler(EditorState state)
        {
            this.state = state;
        }

        public bool HandleInputEvent(InputEvent ev)
        {
            if (state.CurrentToolMode == Brushes.BrushMode.Things)
            {
                if (ev.keycode == Keys.Delete && !ev.down)
                {
                    if (state.SelectedThings.Count > 0)
                    {
                        state.DeleteSelectedThings();
                        return true;
                    }

                }
                else if (ev.keycode == Keys.F)
                {
                    if (ev.down)
                    {
                        if (state.SelectedThings.Count > 0)
                        {
                            state.StartFacing();
                            return true;
                        }
                    }
                    else
                    {
                        state.EndFacing();
                        return true;
                    }
                }
                else if (ev.keycode == Keys.C && ev.down)
                {
                    if (state.CurrentToolMode == Brushes.BrushMode.Things)
                    {
                        state.ClearSelectedThings();
                        return true;
                    }
                }
            }
            else if (state.CurrentToolMode == Brushes.BrushMode.Triggers)
            {
                if (ev.keycode == Keys.Delete && !ev.down)
                {
                    if (state.CurrentLevel.GetTriggers(state.LastOrthoHit.x, state.LastOrthoHit.y, state.LastOrthoHit.z) != null)
                    {
                        state.CurrentLevel.DeleteTriggersAt(state.LastOrthoHit.x, state.LastOrthoHit.y, state.LastOrthoHit.z);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
