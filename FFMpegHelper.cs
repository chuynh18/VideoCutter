using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Note to self:  FFMpeg cut syntax is...
// ffmpeg -ss 00:00:30.0 -i input.wmv -c copy -t 00:00:10.0 output.wmv
// ffmpeg - ss 30 - i input.wmv - c copy - t 10 output.wmv

namespace VideoCutter
{
    /// <summary>
    /// Handles all things FFMpeg:  calling the exe, building the arguments, etc.
    /// </summary>
    class FFMpegHelper
    {

    }
}
