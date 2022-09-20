# school21-xv
### factory simulator
### school project 42/21
### the project was done in crunch mode, based on pattern MVC +- xDD

![Image alt](https://github.com/CaptainKryga/school21-xv/blob/main/git/back.jpg)

The goal of the project is to create a factory simulator:
1. Add the ability to import and export maps.
2. Add the ability to save and load the game with current states.
3. Ability to add objects to the scene and somehow change them (position, rotation, name and color).
4. Record a video of the gameplay and export it from the application to the external folder of the device.
5. Creation of task sequences (moving objects, launching some events in the scene), the ability to change their sequence, run in a loop and change the speed of the task.
6. Player control in 5 variations (first person, third person, spectator mode, worker first person view, worker third person view).

The subject itself: https://github.com/CaptainKryga/school21-xv/blob/main/git/en.subject.pdf

Main control:
WASD or arrows => movement
1, 2, 3, 4, 5 => select surveillance camera
Escape => exit to menu
P => start recording and stop video (bug at the time of delivery, works at any time)

According to the subject, 56 days are given to complete a group of 2-4 people, they did it in crunch mode in 6-7 days in a team of 2 people.

For assembly:
Demlaly project for Unity 2021.3.9f1 on macos
Video recording implemented through ffmpeg configured to work only with macos
