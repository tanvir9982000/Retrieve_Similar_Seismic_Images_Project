/*
* MATLAB Compiler: 6.0 (R2015a)
* Date: Tue Jun 09 15:31:10 2015
* Arguments: "-B" "macro_default" "-W" "dotnet:DotNetProject,Class1,0.0,private" "-T"
* "link:lib" "-d" "D:\Codes\Spatial Envelope\gistdescriptor\DotNetProject\for_testing"
* "-v" "class{Class1:D:\Codes\Spatial Envelope\gistdescriptor\LMgist.m}" 
*/
using System;
using System.Reflection;
using System.IO;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

#if SHARED
[assembly: System.Reflection.AssemblyKeyFile(@"")]
#endif

namespace DotNetProject
{

  /// <summary>
  /// The Class1 class provides a CLS compliant, MWArray interface to the MATLAB
  /// functions contained in the files:
  /// <newpara></newpara>
  /// D:\Codes\Spatial Envelope\gistdescriptor\LMgist.m
  /// </summary>
  /// <remarks>
  /// @Version 0.0
  /// </remarks>
  public class Class1 : IDisposable
  {
    #region Constructors

    /// <summary internal= "true">
    /// The static constructor instantiates and initializes the MATLAB runtime instance.
    /// </summary>
    static Class1()
    {
      if (MWMCR.MCRAppInitialized)
      {
        try
        {
          Assembly assembly= Assembly.GetExecutingAssembly();

          string ctfFilePath= assembly.Location;

          int lastDelimiter= ctfFilePath.LastIndexOf(@"\");

          ctfFilePath= ctfFilePath.Remove(lastDelimiter, (ctfFilePath.Length - lastDelimiter));

          string ctfFileName = "DotNetProject.ctf";

          Stream embeddedCtfStream = null;

          String[] resourceStrings = assembly.GetManifestResourceNames();

          foreach (String name in resourceStrings)
          {
            if (name.Contains(ctfFileName))
            {
              embeddedCtfStream = assembly.GetManifestResourceStream(name);
              break;
            }
          }
          mcr= new MWMCR("",
                         ctfFilePath, embeddedCtfStream, true);
        }
        catch(Exception ex)
        {
          ex_ = new Exception("MWArray assembly failed to be initialized", ex);
        }
      }
      else
      {
        ex_ = new ApplicationException("MWArray assembly could not be initialized");
      }
    }


    /// <summary>
    /// Constructs a new instance of the Class1 class.
    /// </summary>
    public Class1()
    {
      if(ex_ != null)
      {
        throw ex_;
      }
    }


    #endregion Constructors

    #region Finalize

    /// <summary internal= "true">
    /// Class destructor called by the CLR garbage collector.
    /// </summary>
    ~Class1()
    {
      Dispose(false);
    }


    /// <summary>
    /// Frees the native resources associated with this object
    /// </summary>
    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }


    /// <summary internal= "true">
    /// Internal dispose function
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        disposed= true;

        if (disposing)
        {
          // Free managed resources;
        }

        // Free native resources
      }
    }


    #endregion Finalize

    #region Methods

    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray LMgist()
    {
      return mcr.EvaluateFunction("LMgist", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="D">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray LMgist(MWArray D)
    {
      return mcr.EvaluateFunction("LMgist", D);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="D">Input argument #1</param>
    /// <param name="HOMEIMAGES">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray LMgist(MWArray D, MWArray HOMEIMAGES)
    {
      return mcr.EvaluateFunction("LMgist", D, HOMEIMAGES);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="D">Input argument #1</param>
    /// <param name="HOMEIMAGES">Input argument #2</param>
    /// <param name="param_in1">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray LMgist(MWArray D, MWArray HOMEIMAGES, MWArray param_in1)
    {
      return mcr.EvaluateFunction("LMgist", D, HOMEIMAGES, param_in1);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="D">Input argument #1</param>
    /// <param name="HOMEIMAGES">Input argument #2</param>
    /// <param name="param_in1">Input argument #3</param>
    /// <param name="HOMEGIST">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray LMgist(MWArray D, MWArray HOMEIMAGES, MWArray param_in1, MWArray 
                    HOMEGIST)
    {
      return mcr.EvaluateFunction("LMgist", D, HOMEIMAGES, param_in1, HOMEGIST);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] LMgist(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "LMgist", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="D">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] LMgist(int numArgsOut, MWArray D)
    {
      return mcr.EvaluateFunction(numArgsOut, "LMgist", D);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="D">Input argument #1</param>
    /// <param name="HOMEIMAGES">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] LMgist(int numArgsOut, MWArray D, MWArray HOMEIMAGES)
    {
      return mcr.EvaluateFunction(numArgsOut, "LMgist", D, HOMEIMAGES);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="D">Input argument #1</param>
    /// <param name="HOMEIMAGES">Input argument #2</param>
    /// <param name="param_in1">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] LMgist(int numArgsOut, MWArray D, MWArray HOMEIMAGES, MWArray 
                      param_in1)
    {
      return mcr.EvaluateFunction(numArgsOut, "LMgist", D, HOMEIMAGES, param_in1);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the LMgist MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="D">Input argument #1</param>
    /// <param name="HOMEIMAGES">Input argument #2</param>
    /// <param name="param_in1">Input argument #3</param>
    /// <param name="HOMEGIST">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] LMgist(int numArgsOut, MWArray D, MWArray HOMEIMAGES, MWArray 
                      param_in1, MWArray HOMEGIST)
    {
      return mcr.EvaluateFunction(numArgsOut, "LMgist", D, HOMEIMAGES, param_in1, HOMEGIST);
    }


    /// <summary>
    /// Provides an interface for the LMgist function in which the input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// [gist, param] = LMgist(D, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param);
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// For a set of images:
    /// gist = LMgist(img, [], param);
    /// When calling LMgist with a fourth argument it will store the gists in a
    /// new folder structure mirroring the folder structure of the images. Then,
    /// when called again, if the gist files already exist, it will just read
    /// them without recomputing them:
    /// [gist, param] = LMgist(filename, HOMEIMAGES, param, HOMEGIST);
    /// [gist, param] = LMgist(D, HOMEIMAGES, param, HOMEGIST);
    /// Modeling the shape of the scene: a holistic representation of the spatial
    /// envelope
    /// Aude Oliva, Antonio Torralba
    /// International Journal of Computer Vision, Vol. 42(3): 145-175, 2001.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void LMgist(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
    {
      mcr.EvaluateFunction("LMgist", numArgsOut, ref argsOut, argsIn);
    }



    /// <summary>
    /// This method will cause a MATLAB figure window to behave as a modal dialog box.
    /// The method will not return until all the figure windows associated with this
    /// component have been closed.
    /// </summary>
    /// <remarks>
    /// An application should only call this method when required to keep the
    /// MATLAB figure window from disappearing.  Other techniques, such as calling
    /// Console.ReadLine() from the application should be considered where
    /// possible.</remarks>
    ///
    public void WaitForFiguresToDie()
    {
      mcr.WaitForFiguresToDie();
    }



    #endregion Methods

    #region Class Members

    private static MWMCR mcr= null;

    private static Exception ex_= null;

    private bool disposed= false;

    #endregion Class Members
  }
}
