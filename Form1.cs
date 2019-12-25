using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Sarib Kashif
//Mr. Rowbottom
//June 18, 2018
//Program mimicks yahtzee game and has a cheat method which gives doubled chance for three turns
//if the yahtzee picture in game is clicked. Has a start screen, game screen and end screen.
//Scoring possibilities are displayed before user picks them 

/*Critical Requirements (__/20)
*contains appropriate commenting
*appropriate naming conventions of variables and functions
*contain an array to store the dice values
*display the dice as images or buttons
*use a roll function to get new random values
*have a way of re-rolling the dice values
*have boolean variables for 3X, 4X, Yahtzee, fullhouse, small run, large run 
*display a grid to display the scoreboard
*have a boolean array for dice being held
*only reroll dice that are not held
*show the user which dice are hold
*calculate the 1 to 6s on the on the scoreboard
*prevent users from rolling the dice more than three times
*allow users to select a scoring option
*prevent users from re-selecting score options
*calculate the 1s to 6s bonus value and sum the totals
*calculate on the scoreboard for 3X, 4X, Yahtzee
*calculate the scoring on a small and large straight
*calculate the scoring for a full  house and chance
*the game ends appropriately when all turns are used up

Optional Features ( 3 of the optional ) (__/3)
*multiple game screens are displayed. (Could be just different views on the same form)
*secret cheat method 
*scoring possibilities are displayed to the user
*/

namespace Yahtzee 
{
    public partial class YahtzeeGame : Form
    {
        Random randomDice = new Random(); //Random variable for picking a random dice value
        Image[] dice; //Array of Images which will hold all the dices
        int[] dieType = new int[5]; //Holds numbers for the 'dice' array which will display the dice images
        int rollsRemaining = 3;

        //Variables for storing all the different scoring types
        int ones = 0;
        int twos = 0;
        int threes = 0;
        int fours = 0;
        int fives = 0;
        int sixes = 0;
        int sum = 0;
        bool bonus = false;
        int threeOfAKind = 0;
        int fourOfAKind = 0;
        bool smallStraight = false;
        bool largeStraight = false;
        bool fullHouse = false;
        int chance = 0;
        bool yahtzee = false;
        int totalScore = 0;

        //Storing the dice values in arrays
        int [] diceValues = new int[5];
        int[] diceValues2 = new int[5];

        Boolean[] chosenDice = new Boolean[5]; //Array of boolean for storing each dice individually
        int rolls = 0;
        bool[] scoreSelected = new bool[13]; //boolean array for all scoring types, if false, it has not been selected yet, if true, it has been picked already
        bool selection = false;
        bool cheat = false;
        int cheatsLeft = 0;

        public YahtzeeGame()
        {
            InitializeComponent();
            StartGame();
            UpdateLabels();
        }

        private void StartGame()
        {
            //hides all images, buttons, labels, etc which should not be shown in the start screen
            b_1.Visible = false;
            b_2.Visible = false;
            b_3.Visible = false;
            b_4.Visible = false;
            b_5.Visible = false;
            b_yahtzeePicture.Visible = false;
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel2.Visible = false;
            l_rolls.Visible = false;
            l_stored1.Visible = false;
            l_stored2.Visible = false;
            l_stored3.Visible = false;
            l_stored4.Visible = false;
            l_stored5.Visible = false;
            b_rollDice.Visible = false;
            b_homePicture.Enabled = false;
        }

        private void b_startGame_Click(object sender, EventArgs e) 
        {
            //Show all buttons, labels, etc for the actual game and hide the button and image that were in the home screen
            b_1.Visible = true;
            b_2.Visible = true;
            b_3.Visible = true;
            b_4.Visible = true;
            b_5.Visible = true;
            b_yahtzeePicture.Visible = true;
            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = true;
            l_rolls.Visible = true;
            l_stored1.Visible = true;
            l_stored2.Visible = true;
            l_stored3.Visible = true;
            l_stored4.Visible = true;
            l_stored5.Visible = true;
            b_rollDice.Visible = true;
            b_startGame.Visible = false;
            b_homePicture.Visible = false;
            InIt(); //call on init to set up variables
        }

        private void InIt()
        {
            //Disable buttons so that they cant be clicked
            b_1.Enabled = false;
            b_2.Enabled = false;
            b_3.Enabled = false;
            b_4.Enabled = false;
            b_5.Enabled = false;

            //declare array of dice and store an image of each dice 
            dice = new Image[6];
            dice[0] = Yahtzee.Properties.Resources.Dice1;
            dice[1] = Yahtzee.Properties.Resources.Dice2;
            dice[2] = Yahtzee.Properties.Resources.Dice3;
            dice[3] = Yahtzee.Properties.Resources.Dice4;
            dice[4] = Yahtzee.Properties.Resources.Dice5;
            dice[5] = Yahtzee.Properties.Resources.Dice6;

            //hide labels which show if the dice value has been stored because it is not possible for any values to be stored 
            l_stored1.Visible = false;
            l_stored2.Visible = false;
            l_stored3.Visible = false;                
            l_stored4.Visible = false;
            l_stored5.Visible = false;
            //set all chosen dices to true 
            for(int i = 0; i < chosenDice.Length; i++)
            {
                chosenDice[i] = true;
            }
            //set all selected scores to false as no scores have been selected at the start of the game
            for (int i = 0; i < scoreSelected.Length; i++)
            {
                scoreSelected[i] = false;
            }
        }

        public void UpdateLabels()
        {
            //update all labels by displaying the values of each variable incase it changed
            l_ones.Text = "" + ones;
            l_twos.Text = "" + twos;
            l_threes.Text = "" + threes;
            l_fours.Text = "" + fours;
            l_fives.Text = "" + fives;
            l_sixes.Text = "" + sixes;
            l_sum.Text = "" + sum;
            l_bonus.Text = "" + bonus;
            l_threeOfAKind.Text = "" + threeOfAKind;
            l_fourOfAKind.Text = "" + fourOfAKind;
            //show boolean score methods if they are being rolled 
            if (fullHouse)
            {
                l_fullHouse.Text = "25";
            }
            else
            {
                l_fullHouse.Text = "0";
            }
            if (smallStraight)
            {
                l_smallStraight.Text = "30";
            }
            else
            {
                l_smallStraight.Text = "0";
            }
            if (largeStraight)
            {
                l_largeStraight.Text = "40";
            }
            else
            {
                l_largeStraight.Text = "0";
            }
            l_chance.Text = "" + chance;
            if (yahtzee)
            {
                l_yahtzee.Text = "50";
            }
            else
            {
                l_yahtzee.Text = "0";
            }
            if (bonus)
            {
                l_bonus.Text = "35";
            }
            else
            {
                l_bonus.Text = "0";
            }
            l_totalScore.Text = "" + totalScore;
            l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
            //determines if bonus is valid for use
            if (sum >= 63)
            {
                bonus = true;
            }
            //whether or not cheat label should be displayed
            if (cheatsLeft == 0)
            {
                cheat = false;
                l_cheat.Visible = false;
            }
            //if all scores have been selected, play the end game function
            if (scoreSelected[0] && scoreSelected[1] && scoreSelected[2] && scoreSelected[3] && scoreSelected[4] && scoreSelected[5] && scoreSelected[6] && scoreSelected[7] && scoreSelected[8] && scoreSelected[9] && scoreSelected[10] && scoreSelected[11] && scoreSelected[12])
            {
                EndGame();
            }
            if (cheat)
            {
                l_cheat.Text = "Secret Cheat Method Unlocked: Doubled Chance for " + cheatsLeft + " Turns";
            }
        }

        private void EndGame()
        {
            //hide all game items such as dice buttons and score tables
            b_1.Visible = false;
            b_2.Visible = false;
            b_3.Visible = false;
            b_4.Visible = false;
            b_5.Visible = false;
            b_yahtzeePicture.Visible = false;
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel2.Visible = false;
            l_rolls.Visible = false;
            l_stored1.Visible = false;
            l_stored2.Visible = false;
            l_stored3.Visible = false;
            l_stored4.Visible = false;
            l_stored5.Visible = false;
            //display the total score that the user got
            b_rollDice.Text = "Total Score: " + totalScore;
            l_cheat.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rollsRemaining > 0)
            {
                //after user has rolled atleast once, make sure user can store values
                if (rolls == 0)
                {
                    b_1.Enabled = true;
                    b_2.Enabled = true;
                    b_3.Enabled = true;
                    b_4.Enabled = true;
                    b_5.Enabled = true;
                    selection = true;
                }
                //checking if each score type should be reset according to if it has been selected yet
                if(scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                rollDices();
                StoreValues();
                BubbleSort();
                //checks for all score possibilities on right side
                CheckThreeOfAKind(); 
                CheckFourOfAKind();
                CheckFullHouse();
                CheckSmallStraight();
                CheckYahtzee();
                AddChance();
                CheckLargeStraight();
                //adjust rolls
                rollsRemaining--;
                rolls++;
                UpdateLabels();
            }
        }


        private void rollDices()
        {
            GetDiceType(); //get 5 different random values and put in each array
            for (int i = 0; i < 5; i++) //for loop to go through each dice and store value
            {
                if (chosenDice[0]) //if the first dice has not been stored
                {
                    if (i == 0) //for the first dice
                    {
                        b_1.BackgroundImage = dice[dieType[0]]; //set the first dice picture to the random value which was chosen
                        diceValues[0] = (dieType[0] + 1); //store the value in an array (add one because array start at 0)
                    }
                }
                //goes through the rest of the dices and does the same as above
                if (chosenDice[1])
                {
                    if (i == 1)
                    {
                        b_2.BackgroundImage = dice[dieType[1]];
                        diceValues[1] = (dieType[1] + 1);
                    }
                }
                if (chosenDice[2])
                {
                    if (i == 2)
                    {
                        b_3.BackgroundImage = dice[dieType[2]];
                        diceValues[2] = (dieType[2] + 1);
                    }
                }
                if (chosenDice[3])
                {
                    if (i == 3)
                    {
                        b_4.BackgroundImage = dice[dieType[3]];
                        diceValues[3] = (dieType[3] + 1);
                    }
                }
                if (chosenDice[4])
                {
                    if (i == 4)
                    {
                        b_5.BackgroundImage = dice[dieType[4]];
                        diceValues[4] = (dieType[4] + 1);
                    }
                }
                if (dieType[i] == 0 && scoreSelected[0] == false) //if the dice rolled a one, add one to the one label
                {
                    ones++;
                }
                else if (dieType[i] == 1 && scoreSelected[1] == false) //if the dice rolled two, add two to the two label
                {
                    twos += 2;
                }
                else if (dieType[i] == 2 && scoreSelected[2] == false) //do this for every value (1-6)
                {
                    threes += 3;
                }
                else if (dieType[i] == 3 && scoreSelected[3] == false)
                {
                    fours += 4;
                }
                else if (dieType[i] == 4 && scoreSelected[4] == false)
                {
                    fives += 5;
                }
                else if (dieType[i] == 5 && scoreSelected[5] == false)
                {
                    sixes += 6;
                }
            }
        }

        private void BubbleSort() //sorting algorithom to organize the set of values
        {
            int temp = 0;
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (int j = 0; j < 4; j++)
                {
                    if (diceValues2[j] > diceValues2[j + 1])
                    {
                        temp = diceValues2[j];
                        diceValues2[j] = diceValues2[j + 1];
                        diceValues2[j + 1] = temp;
                        sorted = false;
                    }
                }
            }
        }

        private void GetDiceType()
        {
            for(int i = 0; i < dieType.Length; i++)
            {
                if(chosenDice[i] == true) //if the dice is not stored, put a random value between 0 and 5 in the dieType variable
                {
                    dieType[i] = randomDice.Next(0, 6);
                }
            }
        }

        private void CheckThreeOfAKind()
        {
            if (scoreSelected[6] == false) 
            {
                bool TOAK = false;

                for (int i = 1; i <= 6; i++)
                {
                    int numberChecker = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        if (diceValues[j] == i) //if the previous number is equal to the next one, then add 1 to counter
                            numberChecker++;

                        if (numberChecker > 2) //if there are three or more counted, there is a three of a kind
                            TOAK = true;
                    }
                }

                if (TOAK)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        threeOfAKind += diceValues[k]; //add all dice values
                    }
                }
            }
        }

        private void CheckFourOfAKind()
        {
            if (scoreSelected[7] == false)
            {
                //four of a kind is the same as three except now counted must go upto 4 or more
                bool FOAK = false;

                for (int i = 1; i <= 6; i++)
                {
                    int numberChecker = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        if (diceValues[j] == i)
                            numberChecker++;

                        if (numberChecker > 3)
                            FOAK = true;
                    }
                }
                if (FOAK)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        fourOfAKind += diceValues[k];
                    }
                }
            }
        }

        private void CheckLargeStraight()
        {
            if (scoreSelected[10] == false)
            {
                int[] temp = new int[5];

                temp[0] = diceValues2[0]; //temporary array to hold values
                temp[1] = diceValues2[1];
                temp[2] = diceValues2[2];
                temp[3] = diceValues2[3];
                temp[4] = diceValues2[4];
                if (((temp[0] == 1) && //if each value in order goes 1,2,3,4,5 or 2,3,4,5,6, then set large straight to true
                (temp[1] == 2) &&
                (temp[2] == 3) &&
                (temp[3] == 4) &&
                (temp[4] == 5)) ||
               ((temp[0] == 2) &&
                (temp[1] == 3) &&
                (temp[2] == 4) &&
                (temp[3] == 5) &&
                (temp[4] == 6)))
                {
                    largeStraight = true;
                }
            }
        }

        private void CheckFullHouse()
        {
            if (scoreSelected[8] == false)
            {
                int[] temp = new int[5];

                temp[0] = diceValues2[0];
                temp[1] = diceValues2[1];
                temp[2] = diceValues2[2];
                temp[3] = diceValues2[3];
                temp[4] = diceValues2[4];

                if ((((temp[0] == temp[1]) && (temp[1] == temp[2])) && // Three of a Kind
           (temp[3] == temp[4]) && // Two of a Kind
           (temp[2] != temp[3])) ||
          ((temp[0] == temp[1]) && // Two of a Kind
           ((temp[2] == temp[3]) && (temp[3] == temp[4])) && // Three of a Kind
           (temp[1] != temp[2]))) //set full house to true if there is a set of three same and two same
                {
                    fullHouse = true;
                }
            }
        }

        private void CheckSmallStraight()
        {
            if (scoreSelected[9] == false)
            {
                int[] t = new int[5];
                t[0] = diceValues2[0];
                t[1] = diceValues2[1];
                t[2] = diceValues2[2];
                t[3] = diceValues2[3];
                t[4] = diceValues2[4];

                //moves one of two values which are the same to the end of the sequence
                for (int j = 0; j < 4; j++) //if there are any values which are doubles, move one of them to the end
                {
                    int temp = 0;
                    if (t[j] == t[j + 1])
                    {
                        temp = t[j];

                        for (int k = j; k < 4; k++)
                        {
                            t[k] = t[k + 1];
                        }

                        t[4] = temp;
                    }
                }

                if (((t[0] == 1) && (t[1] == 2) && (t[2] == 3) && (t[3] == 4)) ||
          ((t[0] == 2) && (t[1] == 3) && (t[2] == 4) && (t[3] == 5)) ||
          ((t[0] == 3) && (t[1] == 4) && (t[2] == 5) && (t[3] == 6)) || //if there is a set of four consecutive values, then small straight is true
          ((t[1] == 1) && (t[2] == 2) && (t[3] == 3) && (t[4] == 4)) ||
          ((t[1] == 2) && (t[2] == 3) && (t[3] == 4) && (t[4] == 5)) ||
          ((t[1] == 3) && (t[2] == 4) && (t[3] == 5) && (t[4] == 6)))
                {
                    smallStraight = true;
                }
            }
        }

        private void CheckYahtzee()
        {
            if (scoreSelected[12] == false)
            { //same as toak and foak, but counter must go all the way till 5
                for (int i = 1; i <= 6; i++)
                {
                    int numberChecker = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        if (diceValues[j] == i)
                        {
                            numberChecker++;
                        }

                        if (numberChecker > 4)
                        {
                            yahtzee = true;
                        }
                    }
                }
            }
        }

        private void AddChance()
        {
            if (scoreSelected[11] == false)
            {
                if (!cheat) //if the cheat method is not activated, just add the values
                {
                    for (int i = 0; i < 5; i++)
                    {
                        chance += diceValues2[i];
                    }
                }
                else if (cheat && cheatsLeft > 0) //if cheats is activated and there are still more than 0, double the chance
                {
                    for (int i = 0; i < 5; i++)
                    {
                        chance += diceValues2[i];
                    }
                    chance = chance * 2;
                }
            }
        }

        private void b_1_Click(object sender, EventArgs e)
        {
            if (chosenDice[0])
            {
                l_stored1.Visible = true; //if the dice is pressed, show text saying the value is stored and set chosen dice to false
                chosenDice[0] = false;
            }
            else if(!chosenDice[0])
            {
                l_stored1.Visible = false;
                chosenDice[0] = true;
            }
        }

        private void b_2_Click(object sender, EventArgs e)
        {
            if (chosenDice[1])
            {
                l_stored2.Visible = true;
                chosenDice[1] = false;
            }
            else if (!chosenDice[1])
            {
                l_stored2.Visible = false;
                chosenDice[1] = true;
            }
        }

        private void b_3_Click(object sender, EventArgs e)
        {
            if (chosenDice[2])
            {
                l_stored3.Visible = true;
                chosenDice[2] = false;
            }
            else if (!chosenDice[2])
            {
                l_stored3.Visible = false;
                chosenDice[2] = true;
            }
        }

        private void b_4_Click(object sender, EventArgs e)
        {
            if (chosenDice[3])
            {
                l_stored4.Visible = true;
                chosenDice[3] = false;
            }
            else if (!chosenDice[3])
            {
                l_stored4.Visible = false;
                chosenDice[3] = true;
            }
        }

        private void b_5_Click(object sender, EventArgs e)
        {
            if (chosenDice[4])
            {
                l_stored5.Visible = true;
                chosenDice[4] = false;
            }
            else if (!chosenDice[4])
            {
                l_stored5.Visible = false;
                chosenDice[4] = true;
            }
        }

        private void StoreValues()
        {
            for(int i = 0; i < 5; i++)
            {
                diceValues2[i] = diceValues[i];
            }
        }

        private void l_ones_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[0] && selection) //if the label has not yet been picked
            {
                l_ones.ForeColor = Color.Black; //set font color to black and background to gold
                l_ones.BackColor = Color.Gold;
                scoreSelected[0] = true; //score has been selected so make it true
                selection = false;
                rollsRemaining = 3; //reset the rolls
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining; //display the rolls have been reset
                //set all buttons to false and hide all stored labels
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                //reset any label values which weren't chosen yet
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                //add up the 1-6 to make sum
                sum = ones + twos + threes + fours + fives + sixes;
                //reset total score and add up all values
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                //subtract cheats and update labels
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_twos_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[1] && selection)
            {
                l_twos.ForeColor = Color.Black;
                l_twos.BackColor = Color.Gold;
                scoreSelected[1] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_threes_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[2] && selection)
            {
                l_threes.ForeColor = Color.Black;
                l_threes.BackColor = Color.Gold;
                scoreSelected[2] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_fours_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[3] && selection)
            {
                l_fours.ForeColor = Color.Black;
                l_fours.BackColor = Color.Gold;
                scoreSelected[3] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_fives_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[4] && selection)
            {
                l_fives.ForeColor = Color.Black;
                l_fives.BackColor = Color.Gold;
                scoreSelected[4] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_sixes_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[5] && selection)
            {
                l_sixes.ForeColor = Color.Black;
                l_sixes.BackColor = Color.Gold;
                scoreSelected[5] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_threeOfAKind_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[6] && selection)
            {
                l_threeOfAKind.ForeColor = Color.Black;
                l_threeOfAKind.BackColor = Color.Gold;
                scoreSelected[6] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_fourOfAKind_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[7] && selection)
            {
                l_fourOfAKind.ForeColor = Color.Black;
                l_fourOfAKind.BackColor = Color.Gold;
                scoreSelected[7] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_fullHouse_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[8] && selection)
            {
                l_fullHouse.ForeColor = Color.Black;
                l_fullHouse.BackColor = Color.Gold;
                scoreSelected[8] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_smallStraight_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[9] && selection)
            {
                l_smallStraight.ForeColor = Color.Black;
                l_smallStraight.BackColor = Color.Gold;
                scoreSelected[9] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_largeStraight_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[10] && selection)
            {
                l_largeStraight.ForeColor = Color.Black;
                l_largeStraight.BackColor = Color.Gold;
                scoreSelected[10] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_chance_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[11] && selection)
            {
                l_chance.ForeColor = Color.Black;
                l_chance.BackColor = Color.Gold;
                scoreSelected[11] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void l_yahtzee_Click(object sender, EventArgs e)
        {
            if (!scoreSelected[12] && selection)
            {
                l_yahtzee.ForeColor = Color.Black;
                l_yahtzee.BackColor = Color.Gold;
                scoreSelected[12] = true;
                selection = false;
                rollsRemaining = 3;
                rolls = 0;
                l_rolls.Text = "Rolls Remaining: " + rollsRemaining;
                b_1.Enabled = false;
                b_2.Enabled = false;
                b_3.Enabled = false;
                b_4.Enabled = false;
                b_5.Enabled = false;
                l_stored1.Visible = false;
                l_stored2.Visible = false;
                l_stored3.Visible = false;
                l_stored4.Visible = false;
                l_stored5.Visible = false;
                for (int i = 0; i < chosenDice.Length; i++)
                {
                    chosenDice[i] = true;
                }
                if (scoreSelected[0] == false)
                {
                    ones = 0;
                }
                if (scoreSelected[1] == false)
                {
                    twos = 0;
                }
                if (scoreSelected[2] == false)
                {
                    threes = 0;
                }
                if (scoreSelected[3] == false)
                {
                    fours = 0;
                }
                if (scoreSelected[4] == false)
                {
                    fives = 0;
                }
                if (scoreSelected[5] == false)
                {
                    sixes = 0;
                }
                if (scoreSelected[6] == false)
                {
                    threeOfAKind = 0;
                }
                if (scoreSelected[7] == false)
                {
                    fourOfAKind = 0;
                }
                if (scoreSelected[8] == false)
                {
                    fullHouse = false;
                }
                if (scoreSelected[9] == false)
                {
                    smallStraight = false;
                }
                if (scoreSelected[10] == false)
                {
                    largeStraight = false;
                }
                if (scoreSelected[11] == false)
                {
                    chance = 0;
                }
                if (scoreSelected[12] == false)
                {
                    yahtzee = false;
                }
                totalScore = 0;
                totalScore = ones + twos + threes + fours + fives + sixes + threeOfAKind + fourOfAKind + chance;
                if (yahtzee)
                {
                    totalScore += 50;
                }
                if (fullHouse)
                {
                    totalScore += 25;
                }
                if (smallStraight)
                {
                    totalScore += 30;
                }
                if (largeStraight)
                {
                    totalScore += 40;
                }
                if (bonus)
                {
                    totalScore += 35;
                }
                sum = ones + twos + threes + fours + fives + sixes;
                if (sum < 63)
                {
                    bonus = false;
                }
                cheatsLeft--;
                UpdateLabels();
            }
        }

        private void b_yahtzeePicture_Click(object sender, EventArgs e)
        {
            if (!cheat) //if the cheat has not been activated
            {
                cheat = true; //set cheat to true
                l_cheat.Visible = true; //show the cheats label
                cheatsLeft = 3; //set cheats to 3
                b_yahtzeePicture.Enabled = false; //make sure the picture can no longer be clicked
            }
        }
























        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void l_rolls_Click(object sender, EventArgs e)
        {

        }

        private void l_sum_Click(object sender, EventArgs e)
        {

        }

        private void YahtzeeGame_Load(object sender, EventArgs e)
        {

        }
    }
}
