package presentation;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;

public class Main extends Application {

    @Override
    public void start(Stage primaryStage) throws Exception{
        final FXMLLoader loader = new FXMLLoader(
                getClass().getResource(
                        "application.fxml"
                )
        );

        final Parent root = (Parent) loader.load();

        final Controller controller = loader.getController();
        controller.initialize(primaryStage);

        primaryStage.setTitle("Herrings");
        primaryStage.setScene(new Scene(root, 680, 660));
        primaryStage.show();
    }


    public static void main(String[] args) {
        launch(args);
    }
}
