<?php
/*
 * This is a sample php class file
 */

class Foo
{
    /**
     * This is a text field
     * @var string
     */
    private $text;

    public function __construct()
    {
        $this->text = "sample";
    }

    /**
     * Get the text field.
     * @return string The text field.
     */
    public function getText()
    {
        return $this->text;
    }
    
    
    
    /**
     * Set the text field.
     * @param string $text
     */
    public function setText($text)
    {
        $this->text = $text;

    }

}