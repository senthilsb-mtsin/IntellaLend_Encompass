import java.io.File;

import org.jdom.Document;
import com.ephesoft.dcma.script.IJDomScript;
import com.ephesoft.dcma.util.logger.EphesoftLogger;
import com.ephesoft.dcma.util.logger.ScriptLoggerFactory;

/**
 * The <code>ScriptPageProcessing</code> class represents the script execute structure. Writer of scripts plug-in should implement this
 * IScript interface to execute it from the scripting plug-in. Via implementing this interface writer can change its java file at run
 * time. Before the actual call of the java Scripting plug-in will compile the java and run the new class file.
 * 
 * @author Ephesoft
 * @version 1.0
 */
public class ScriptPageProcessing implements IJDomScript {

	private static final EphesoftLogger LOGGER = ScriptLoggerFactory.getLogger(ScriptPageProcessing.class);

	/**
	 * The <code>execute</code> method will execute the script written by the writer at run time with new compilation of java file. It
	 * will execute the java file dynamically after new compilation.
	 * 
	 * @param document {@link Document}
	 */
	public Object execute(Document document, String methodName, String documentIdentifier) {
		Exception exception = null;
		try {
			LOGGER.info("*************  Inside ScriptPageProcessing scripts.");
			LOGGER.info("*************  Start execution of the ScriptPageProcessing scripts.");
			LOGGER.info("Hello World.");
			LOGGER.info("*************  Successfully write the xml file for the ScriptPageProcessing scripts.");
			LOGGER.info("*************  End execution of the ScriptPageProcessing scripts.");
		} catch (Exception e) {
			LOGGER.error("*************  Error occurred in scripts." + e.getMessage());
			exception = e;
		}
		return null;
	}
}
